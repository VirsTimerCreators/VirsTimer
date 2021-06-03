package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Session
import pl.virstimer.repository.SessionRepository

@RestController
@RequestMapping("/session")
class SessionController(val repository: SessionRepository) {

    @GetMapping("/all")
    fun findAllSession(): MutableList<Session> = repository.findAll()

    @GetMapping("/user/{userId}")
    fun findAllUserId(@PathVariable userId: String): List<Session> = repository.findAllByUserId(userId)

    @GetMapping("/event/{eventId}")
    fun findAllEventId(@PathVariable eventId: String): List<Session> = repository.findAllByEventId(eventId)

    @GetMapping("/{eventId}")
    fun getSession(@PathVariable eventId: ObjectId) {

    }


    @PostMapping
    fun createController(@RequestBody request: SessionRequest): ResponseEntity<Session> {
        val session = repository.save(
            Session(
                id = request.id,
                userId = request.userId,
                eventId = request.eventId,
                name = request.name
            )
        )
        return ResponseEntity(session, HttpStatus.CREATED)
    }

}

data class SessionRequest(
    val id: ObjectId,
    val userId: String,
    val eventId: String,
    val name: String
)