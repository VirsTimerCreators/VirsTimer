package pl.virstimer.service

import org.springframework.data.repository.findByIdOrNull
import org.springframework.stereotype.Service
import pl.virstimer.api.multiplayer.CreateRoomRequest
import pl.virstimer.api.multiplayer.RoomResponse
import pl.virstimer.api.multiplayer.RoomStatus
import pl.virstimer.api.multiplayer.ScrambleResponse
import pl.virstimer.model.Solved
import pl.virstimer.model.multiplayer.PersistentRoom
import pl.virstimer.model.multiplayer.PersistentScramble
import pl.virstimer.repository.multiplayer.MultiplayerSolveRepository
import pl.virstimer.repository.multiplayer.RoomRepository
import java.util.*

@Service
class RoomService(
    private val roomRepository: RoomRepository,
    private val scrambleService: ScrambleService,
    private val solveRepository: MultiplayerSolveRepository
) {

    fun getRoomWithSolves(roomId: String, userId: String): PeristentRoomWithSolves {
        val room = roomRepository.findByIdAndUser(roomId, userId) ?: throw RuntimeException("RoomId $roomId not found")

        val solvesForUsers = solveRepository.findByIds(room.users, roomId)

        val users = room.users
        val usersWithSolves = solvesForUsers.map { it.userId }.distinct()

        val usersWithoutSolves = users - usersWithSolves.toSet()

        val solvesMap = usersWithoutSolves.associate { it to emptyList<SolveInfo>() } + solvesForUsers
            .map { SolveInfo(it.id, it.userId, it.roomId, it.scrambleId, it.time, it.timestamp, it.solved)}
            .groupBy { it.userId }

        return PeristentRoomWithSolves(
            room.id,
            room.scrambleIds,
            room.status,
            room.administratorId,
            solvesMap,
            room.joinCode
        )
    }

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
            scrambles.map { ScrambleResponse(it.id, it.scrambleText, it.scrambleSvg) },
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

    data class PeristentRoomWithSolves(
        val id: String,
        val scrambleIds: Set<String>,
        val status: RoomStatus,
        val administratorId: String,
        val users: Map<String, List<SolveInfo>>,
        val joinCode: String
    )

    data class SolveInfo(
        val id: String,
        val userId: String,
        val roomId: String,
        val scrambleId: String,
        val time: Long,
        val timestamp: Long,
        val solved: Solved
    )

}
