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


interface SessionRepository: MongoRepository<Session, ObjectId> {
    fun findOneByIdAndUserId(id: ObjectId, userId: String): Session
    fun findAllByUserId(userId: String): List<Session>
    fun findByEventIdAndUserId(eventId: String, userId: String): List<Session>
    fun deleteSessionById(id: ObjectId)
    fun deleteAllByUserId(userId: String)

}
interface SessionCustomRepository{
    fun updateSession(id :ObjectId, sessionUpdate: SessionChange, userId: String)
}
@Repository
class SessionCustomRepositoryImpl(
    val mongoTemplate: MongoTemplate
):SessionCustomRepository{
    override fun updateSession(id: ObjectId, sessionUpdate: SessionChange, userId: String) {
        mongoTemplate.updateFirst(
            Query(Criteria("_id").`is`(id).and("userId").`is`(userId)),
            Update().set("name", sessionUpdate.name),
            Session::class.java
        )
    }
}