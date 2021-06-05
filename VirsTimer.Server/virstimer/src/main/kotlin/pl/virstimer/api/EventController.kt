package pl.virstimer.api

import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.security.access.prepost.PreAuthorize
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Event
import pl.virstimer.repository.EventRepository
import java.util.*

@RestController()
internal class EventController(val repository: EventRepository) {


    @GetMapping("/event/user/{userId}")
    fun findAllForUser(@PathVariable userId: String): List<Event> = repository.findByUserId(userId)

    @PostMapping("/event")
    @PreAuthorize("hasRole('USER')")
    fun createEvent(@RequestBody request: EventRequest): ResponseEntity<Event> {
        val event = repository.save(
            Event(
                id = UUID.randomUUID().toString(),
                userId = request.userId,
                puzzleType = request.puzzleType
            )
        )
        return ResponseEntity(event, HttpStatus.CREATED)
    }
}

data class EventRequest(
    val userId: String,
    val puzzleType: String
)