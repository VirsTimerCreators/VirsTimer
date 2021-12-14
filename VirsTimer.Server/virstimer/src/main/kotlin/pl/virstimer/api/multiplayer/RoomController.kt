package pl.virstimer.api.multiplayer

import com.google.gson.Gson
import org.springframework.http.codec.ServerSentEvent
import org.springframework.security.access.annotation.Secured
import org.springframework.security.core.Authentication
import org.springframework.web.bind.annotation.*
import pl.virstimer.domain.PuzzleType
import pl.virstimer.model.multiplayer.PersistentScramble
import pl.virstimer.service.RoomService
import reactor.core.publisher.Flux
import java.time.Duration


@RestController
@RequestMapping("/room")
class RoomController(
    val roomService: RoomService
) {

    @PostMapping
    @Secured("ROLE_USER")
    fun createRoom(@RequestBody createRoomRequest: CreateRoomRequest, authentication: Authentication): RoomResponse =
        roomService.createRoom(createRoomRequest, authentication.name)

    @PatchMapping("/{roomId}")
    @Secured("ROLE_USER")
    fun modifyRoomStatus(@PathVariable roomId: String, @RequestBody changeRoomStatus: ChangeRoomStatusRequest, authentication: Authentication): Boolean =
        roomService.modifyRoomStatus(roomId, changeRoomStatus.newStatus, authentication.name)

    @PostMapping("/join")
    @Secured("ROLE_USER")
    fun joinRoom(@RequestBody joinRequest: JoinRequest, authentication: Authentication): RoomResponse =
        roomService.joinRoom(joinRequest.joinCode, authentication.name)

    @PostMapping("/leave")
    @Secured("ROLE_USER")
    fun leaveRoom(@RequestBody leaveRequest: LeaveRequest, authentication: Authentication) =
        roomService.leaveRoom(leaveRequest.roomId, authentication.name)

    @GetMapping("/{roomId}/scrambles")
    @Secured("ROLE_USER")
    fun getScramblesForRoom(@PathVariable roomId: String, authentication: Authentication): List<PersistentScramble> =
        roomService.getScrambles(roomId)

    @GetMapping("/{roomId}/feed")
    @Secured("ROLE_USER")
    fun streamEvents(@PathVariable roomId: String, authentication: Authentication): Flux<ServerSentEvent<String>> {
        return Flux.interval(Duration.ofSeconds(4))
            .map { sequence: Long ->
                ServerSentEvent.builder<String>()
                    .id(sequence.toString())
                    .event("periodic-event")
                    .data( Gson().toJson(roomService.getRoom(roomId, authentication.name)).toString() )
                    .build()
            }
            .onErrorMap { e -> throw RuntimeException("Exception during feed subscription: $e") }
    }
}

enum class RoomStatus { CLOSED, INPROGRESS, OPEN}

data class RoomResponse(
    val id: String,
    val scrambles: List<ScrambleResponse>,
    val status: RoomStatus,
    val administratorId: String,
    val users: Set<String>,
    val joinCode: String
)

data class ScrambleResponse(
    val id: String,
    val scramble: String,
    val scrambleSvg: String
)

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