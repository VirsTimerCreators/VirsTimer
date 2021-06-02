package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.PostMapping
import org.springframework.web.bind.annotation.RequestBody
import org.springframework.web.bind.annotation.RestController
import pl.virstimer.model.Event
import pl.virstimer.repository.EventRepository

@RestController()
internal class EventController(val repository: EventRepository) {


    @PostMapping("/events/post")
    fun createEvent(@RequestBody request: EventRequest): ResponseEntity<Event> {
        val event = repository.save(
            Event(
                id = request.id,
                userId = request.userId,
                puzzleType = request.puzzleType
            )
        )
        return ResponseEntity(event, HttpStatus.CREATED)
    }
}

data class EventRequest (
    val id: ObjectId,
    val userId: String,
    val puzzleType: String)