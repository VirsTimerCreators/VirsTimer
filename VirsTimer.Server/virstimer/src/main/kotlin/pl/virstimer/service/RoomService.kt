package pl.virstimer.service

import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service
import pl.virstimer.api.multiplayer.CreateRoomRequest
import pl.virstimer.api.multiplayer.RoomStatus
import pl.virstimer.model.multiplayer.PersistentRoom
import pl.virstimer.model.multiplayer.PersistentScramble
import pl.virstimer.repository.multiplayer.RoomRepository
import java.util.*

@Service
class RoomService(
    val roomRepository: RoomRepository,
    val scrambleService: ScrambleService
) {

    fun createRoom(createRoomRequest: CreateRoomRequest, userId: String): PersistentRoom {
        val scrambles = scrambleService.createPersistentScrambles(createRoomRequest.scrambleType, createRoomRequest.numberOfScrambles)

        return roomRepository.insert(
            PersistentRoom(
                UUID.randomUUID().toString(),
                scrambles.map { it.id }.toSet(),
                RoomStatus.OPEN,
                userId,
                setOf(userId),
                UUID.randomUUID().toString()
            )
        )
    }

    fun modifyRoomStatus(id: String, status: RoomStatus, userId: String): Boolean =
        roomRepository.modifyRoom(id, status, userId)

    fun joinRoom(joinCode: String, userId: String): Boolean =
        roomRepository.join(joinCode, userId)

    fun leaveRoom(roomId: String, userId: String): Boolean =
        roomRepository.leave(roomId, userId)

    fun getScrambles(roomId: String): List<PersistentScramble> {
        val room = roomRepository.findByIdOrNull(roomId)
        if (room == null) {
            return emptyList()
        }

        return scrambleService.findScramblesById(room.scrambleIds)
    }


}
