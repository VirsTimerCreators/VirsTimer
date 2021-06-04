package pl.virstimer.repository

import org.bson.types.ObjectId
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.core.query.Update
import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.api.SessionChange
import pl.virstimer.model.Session


interface SessionRepository : MongoRepository<Session, String> {
    fun findOneById(id: String): Session
    fun findAllByUserId(UserId: String): List<Session>
    fun findAllByEventId(eventId: String): List<Session>
    fun findByEventIdAndUserId(eventId: String, UserId: String): List<Session>
    fun deleteSessionById(id: String)
}

interface SessionCustomRepository {
    fun updateSession(id: String, sessionUpdate: SessionChange)
}

@Repository
class SessionCustomRepositoryImpl(
    val mongoTemplate: MongoTemplate
) : SessionCustomRepository {
    override fun updateSession(id: String, sessionUpdate: SessionChange) {
        mongoTemplate.updateFirst(
            Query(Criteria("_id").`is`(id)),
            Update().set("name", sessionUpdate.name),
            Session::class.java
        )
    }
}