package pl.virstimer.repository

import org.bson.types.ObjectId
import org.springframework.data.mongodb.repository.MongoRepository
import pl.virstimer.model.Session


interface SessionRepository: MongoRepository<Session, ObjectId> {
    fun findOneById(id: ObjectId): Session
    fun findAllByUserId(UserId: String): List<Session>
    fun findAllByEventId(eventId: String): List<Session>



}