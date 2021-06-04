package pl.virstimer.api

import org.bson.types.ObjectId
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.*
import pl.virstimer.model.Session
import pl.virstimer.model.SessionChange
import pl.virstimer.model.Solve
import pl.virstimer.repository.SessionRepository
import pl.virstimer.repository.SolveCustomRepository

@RestController
@RequestMapping("/sessions")
class SessionController(
    val repository: SessionRepository,

) {

    @GetMapping("/all")
    fun findAllSessions(): List<Session> = repository.findAll()

    @GetMapping("/{sessionId}")
    fun getSession(@PathVariable sessionId: ObjectId): Session = repository.findOneById(sessionId)
    @GetMapping("/hex/{sessionId}")
    fun getSessionHex(@PathVariable sessionId: ObjectId): Session = repository.findOneById(ObjectId(sessionId.toHexString()))

    @GetMapping("/user/{userId}")
    fun findAllUserId(@PathVariable userId: String): List<Session> = repository.findAllByUserId(userId)

    @GetMapping("/event/{eventId}")
    fun findAllEventId(@PathVariable eventId: String): List<Session> = repository.findAllByEventId(eventId)


    @PostMapping("/post")
    fun createSession(@RequestBody request: SessionRequest): ResponseEntity<Session> {
        val session = repository.save(
            Session(
                userId = request.userId,
                eventId = request.eventId,
                name = request.name
            )
        )
        return ResponseEntity(session, HttpStatus.CREATED)
    }    @PostMapping("/hex/post")
    fun createSessionHex(@RequestBody request: SessionRequest): ResponseEntity<Session> {
        val session = repository.save(
            Session(
                //id = request.id,
                id = ObjectId("60ce14080000000000000000"),
                userId = request.userId,
                eventId = request.eventId,
                name = request.name
            )
        )
        return ResponseEntity(session, HttpStatus.CREATED)
    }

    @PatchMapping("/patch/{sessionId}")
    fun updateSession(@PathVariable sessionId: ObjectId, @RequestBody session: SessionChange): ResponseEntity<Session> {

        val original = repository.findOneById(ObjectId(sessionId.toHexString()) )

        val updatedSession = repository.save(
            Session(
                id = original.id,
                userId = original.userId,
                eventId = original.eventId,
                name = session.name
                )
        )
        return ResponseEntity.ok(updatedSession)
    }
    @DeleteMapping("delete/{sessionId}")
    fun deleteSession(@PathVariable sessionId: ObjectId) = repository.deleteSessionById(sessionId)


    @DeleteMapping("delete/all")
    fun deleteSession(){
       repository.deleteAll()
    }
}

data class SessionRequest(
    val userId: String,
    val eventId: String,
    val name: String
)