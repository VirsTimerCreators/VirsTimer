package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Solve
import pl.virstimer.model.Solved
import pl.virstimer.repository.SolveRepository


@RestController
@RequestMapping("/solves")
class SolveController(val repository: SolveRepository) {

    @GetMapping("/all")
    fun findAllSession(): List<Solve> = repository.findAll()

    @GetMapping("/user/{userId}")
    fun findAllUserId(@PathVariable userId: String): List<Solve> = repository.findAllByUserId(userId)

    @GetMapping("/sessions/{sessionId}")
    fun findAllEventId(@PathVariable sessionId: String): List<Solve> = repository.findAllBySessionId(sessionId)

    @GetMapping("/{solveId}")
    fun getSession(@PathVariable solveId: ObjectId) {

    }


    @PostMapping
    fun createEvent(@RequestBody request: SolveRequest): ResponseEntity<Solve> {
        val solve = repository.save(
            Solve(
                userId = request.userId,
                sessionId = request.sessionId,
                scramble = request.scramble,
                time = request.time,
                timestamp = request.timestamp,
                solved = request.solved
            )
        )
        return ResponseEntity(solve, HttpStatus.CREATED)
    }

}

data class SolveRequest(
    val id: ObjectId,
    val userId: String,
    val sessionId: String,
    val scramble: String,
    val time: Long,
    val timestamp: Long,
    val solved: Solved
)
