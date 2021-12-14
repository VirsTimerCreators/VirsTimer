package pl.virstimer.service

import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service
import pl.virstimer.api.multiplayer.CreateRoomRequest
import pl.virstimer.api.multiplayer.RoomResponse
import pl.virstimer.api.multiplayer.RoomStatus
import pl.virstimer.api.multiplayer.ScrambleResponse
import pl.virstimer.model.multiplayer.PersistentRoom
import pl.virstimer.model.multiplayer.PersistentScramble
import pl.virstimer.repository.multiplayer.RoomRepository
import java.util.*

@Service
class RoomService(
    private val roomRepository: RoomRepository,
    private val scrambleService: ScrambleService
) {

    fun getRoom(roomId: String, user: String): PersistentRoom =
        roomRepository.findByIdAndUser(roomId, user) ?: throw RuntimeException("RoomId $roomId not found")

    fun createRoom(createRoomRequest: CreateRoomRequest, userId: String): RoomResponse {
        val newScrambles = scrambleService.createPersistentScrambles(createRoomRequest.scrambleType, createRoomRequest.numberOfScrambles)

        return roomRepository.insert(
            PersistentRoom(
                UUID.randomUUID().toString(),
                newScrambles.map { it.id }.toSet(),
                RoomStatus.OPEN,
                userId,
                setOf(userId),
                UUID.randomUUID().toString()
            )
        ).toRoomResponse()

    }

    fun modifyRoomStatus(id: String, status: RoomStatus, userId: String): Boolean =
        roomRepository.modifyRoom(id, status, userId)

    fun joinRoom(joinCode: String, userId: String): RoomResponse =
        roomRepository.join(joinCode, userId)?.toRoomResponse() ?: throw RuntimeException("Unable to join")

    fun PersistentRoom.toRoomResponse(): RoomResponse {
        val scrambles = scrambleService.findScramblesById(this.scrambleIds)

        return RoomResponse(
            this.id,
            scrambles.map { ScrambleResponse(it.scrambleText, it.scrambleSvg) },
            this.status,
            this.administratorId,
            this.users,
            this.joinCode
        )
    }

    fun leaveRoom(roomId: String, userId: String): Boolean =
        roomRepository.leave(roomId, userId)

    fun getScrambles(roomId: String): List<PersistentScramble> {
        val room = roomRepository.findByIdOrNull(roomId) ?: return emptyList()

        return scrambleService.findScramblesById(room.scrambleIds)
    }


}
