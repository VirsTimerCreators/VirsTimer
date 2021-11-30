package pl.virstimer.api

import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.security.access.annotation.Secured
import org.springframework.security.core.Authentication
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

    @GetMapping
    fun findUserSolves(authentication: Authentication): List<Solve> {
        return repository.findAllByUserId(authentication.name)
    }

    @GetMapping("/{id}")
    @Secured("ROLE_USER")
    fun getSessionById(@PathVariable id: String, authentication: Authentication): Solve = repository.findOneByIdAndUserId(id, authentication.name)

    @GetMapping("/session/{sessionId}")
    @Secured("ROLE_USER")
    fun findAllSolvesForSession(@PathVariable sessionId: String, authentication: Authentication): List<Solve> = repository.findAllBySessionIdAndUserId(sessionId, authentication.name)

    @PostMapping
    @Secured("ROLE_USER")
    fun createSolve(@RequestBody request: SolveRequest, authentication: Authentication): ResponseEntity<Solve> {
        val solve = repository.save(
            Solve(
                id = UUID.randomUUID().toString(),
                userId = authentication.name,
                sessionId = request.sessionId,
                scramble = request.scramble,
                time = request.time,
                timestamp = request.timestamp,
                solved = request.solved
            )
        )
        return ResponseEntity(solve, HttpStatus.CREATED)
    }
    @PostMapping("/many")
    @Secured("ROLE_USER")
    fun createSolves(@RequestBody request: List<SolveRequest>, authentication: Authentication):ResponseEntity<MutableList<Solve>> {
        var solves = mutableListOf<Solve>()
        request.forEach{
            solves.add(
                Solve(
                    id = UUID.randomUUID().toString(),
                    userId = authentication.name,
                    sessionId = it.sessionId,
                    scramble = it.scramble,
                    time = it.time,
                    timestamp = it.timestamp,
                    solved = it.solved
                )
            )
        }
        solves = repository.saveAll(solves)
        return ResponseEntity(solves, HttpStatus.CREATED)
    }

    @PatchMapping("/{solveId}")
    @Secured("ROLE_USER")
    fun updateSolve(@PathVariable solveId: String, @RequestBody solveChange: SolveChange, authentication: Authentication): ResponseEntity<Unit> {
        customRepository.updateSolve(solveId, solveChange, authentication.name)
        return ResponseEntity.ok(Unit)
    }

    @DeleteMapping("{solveId}")
    @Secured("ROLE_USER")
    fun deleteSolve(@PathVariable solveId: String, authentication: Authentication) = repository.deleteSolveByIdAndUserId(solveId, authentication.name)

    @DeleteMapping
    @Secured("ROLE_USER")
    fun deleteSolves(authentication: Authentication) = repository.deleteSolveByUserId(authentication.name)
}

data class SolveRequest(
    val sessionId: String,
    val scramble: String,
    val time: Long,
    val timestamp: Long,
    val solved: Solved
)
