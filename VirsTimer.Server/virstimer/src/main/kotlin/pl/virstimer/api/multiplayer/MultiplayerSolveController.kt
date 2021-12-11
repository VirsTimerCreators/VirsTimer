package pl.virstimer.api.multiplayer


import org.springframework.security.core.Authentication
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Solved
import pl.virstimer.model.multiplayer.MultiplayerSolve
import pl.virstimer.repository.multiplayer.MultiplayerSolveRepository
import java.util.*

@RestController
@RequestMapping("/room/{roomId}/solve")
class MultiplayerSolveController(
    val repository: MultiplayerSolveRepository
) {
    @PostMapping
    fun createMultiplayerSolve(@RequestBody request: CreateMultiplayerSolveRequest, @PathVariable roomId: String, authentication: Authentication) =
        repository.insert(
            MultiplayerSolve(
                id = UUID.randomUUID().toString(),
                userId = authentication.name,
                roomId = roomId,
                scrambleId = request.scrambleId,
                time = request.time,
                timestamp = request.timestamp,
                solved = request.solved
            )
        )
}

data class CreateMultiplayerSolveRequest(
    val scrambleId: String,
    val time: Long,
    val timestamp: Long,
    val solved: Solved
)