package pl.virstimer.api.multiplayer

import org.springframework.security.core.Authentication
import org.springframework.web.bind.annotation.*
import pl.virstimer.domain.PuzzleType
import pl.virstimer.model.multiplayer.PersistentRoom
import pl.virstimer.model.multiplayer.PersistentScramble
import pl.virstimer.service.RoomService

@RestController
@RequestMapping("/room")
class RoomController(
    val roomService: RoomService
) {

    @PostMapping
    fun createRoom(@RequestBody createRoomRequest: CreateRoomRequest, authentication: Authentication): PersistentRoom =
        roomService.createRoom(createRoomRequest, authentication.name)

    @PatchMapping("/{roomId}")
    fun modifyRoomStatus(@PathVariable roomId: String, @RequestBody changeRoomStatus: ChangeRoomStatusRequest, authentication: Authentication): Boolean =
        roomService.modifyRoomStatus(roomId, changeRoomStatus.newStatus, authentication.name)

    @PostMapping("/join")
    fun joinRoom(@RequestBody joinRequest: JoinRequest, authentication: Authentication) =
        roomService.joinRoom(joinRequest.joinCode, authentication.name)

    @PostMapping("/leave")
    fun leaveRoom(@RequestBody leaveRequest: LeaveRequest, authentication: Authentication) =
        roomService.leaveRoom(leaveRequest.roomId, authentication.name)

    @GetMapping("/{roomId}/scrambles")
    fun getScramblesForRoom(@PathVariable roomId: String, authentication: Authentication): List<PersistentScramble> =
        roomService.getScrambles(roomId)
}

enum class RoomStatus { CLOSED, INPROGRESS, OPEN}

data class LeaveRequest(
    val roomId: String
)

data class JoinRequest(
    val joinCode: String
)

data class CreateRoomRequest(
    val numberOfScrambles: Int,
    val scrambleType: PuzzleType
)

data class ChangeRoomStatusRequest(
    val newStatus: RoomStatus
)