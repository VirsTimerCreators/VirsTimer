package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Solve
import pl.virstimer.model.SolveChange
import pl.virstimer.model.Solved

import pl.virstimer.repository.SolveRepository


@RestController
@RequestMapping("/solves")
class SolveController(val repository: SolveRepository) {


    @GetMapping("/all")
    fun findAllSolves(): List<Solve> = repository.findAll()

    @GetMapping("/{objectId}")
    fun getSolve(@PathVariable objectId: ObjectId): Solve = repository.findOneById(objectId)

    @GetMapping("/user/{userId}")
    fun findAllUser(@PathVariable userId: String): List<Solve> = repository.findAllByUserId(userId)

    @GetMapping("/session/{sessionId}")
    fun findAllSession(@PathVariable sessionId: String): List<Solve> = repository.findAllBySessionId(sessionId)


    @PostMapping
    fun createSolve(@RequestBody request: SolveRequest): ResponseEntity<Solve> {
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


    @PatchMapping("/patch/{solvedId}")
    fun updateSolve(@PathVariable solvedId: ObjectId, @RequestBody solve: SolveChange): ResponseEntity<Solve> {

        val original = repository.findOneById(solvedId)

        val updatedSolve = repository.save(
            Solve(
                id = original.id,
                userId = original.userId,
                sessionId = original.sessionId,
                scramble = original.scramble,
                time = original.time,
                timestamp = original.timestamp,
                solved = solve.solved
            )
        )
        return ResponseEntity.ok(updatedSolve)
    }

    @DeleteMapping("delete/{solveId}")
    fun deleteSolve(@PathVariable solveId: ObjectId) = repository.deleteSolveById(solveId)

    @DeleteMapping("delete/all")
    fun deleteSolves(){
        repository.deleteAll()
    }

}

data class SolveRequest(
    val userId: String,
    val sessionId: String,
    val scramble: String,
    val time: Long,
    val timestamp: Long,
    val solved: Solved
)
