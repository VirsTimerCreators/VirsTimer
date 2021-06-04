package pl.virstimer.api

import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Solve
import pl.virstimer.model.SolveChange
import pl.virstimer.model.Solved
import pl.virstimer.repository.SolveCustomRepository

import pl.virstimer.repository.SolveRepository
import java.util.*


@RestController
@RequestMapping("/solve")
class SolveController(
    val repository: SolveRepository,
    val customRepository: SolveCustomRepository) {

    @GetMapping("/{id}")
    fun getSolve(@PathVariable id: String): Solve = repository.findOneById(id)

    @GetMapping("/user/{userId}")
    fun findAllUser(@PathVariable userId: String): List<Solve> = repository.findAllByUserId(userId)

    @GetMapping("/session/{sessionId}")
    fun findBySessionId(@PathVariable sessionId: String): List<Solve> = repository.findAllBySessionId(sessionId)

    @PostMapping
    fun createSolve(@RequestBody request: SolveRequest): ResponseEntity<Solve> {
        val solve = repository.save(
            Solve(
                id = UUID.randomUUID().toString(),
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


    @PatchMapping("/{solveId}")
    fun updateSolve(@PathVariable solveId: String, @RequestBody solveChange: SolveChange): ResponseEntity<Unit> {
        customRepository.updateSolve(solveId, solveChange)
        return ResponseEntity.ok(Unit)
    }

    @DeleteMapping("/{solveId}")
    fun deleteSolve(@PathVariable solveId: String) = repository.deleteSolveById(solveId)
}

data class SolveRequest(
    val userId: String,
    val sessionId: String,
    val scramble: String,
    val time: Long,
    val timestamp: Long,
    val solved: Solved
)
