package pl.virstimer.repository

import org.bson.types.ObjectId
import org.springframework.data.mongodb.repository.MongoRepository
import pl.virstimer.model.Session
import pl.virstimer.model.Solve

interface SolveRepository : MongoRepository<Solve, ObjectId> {
    fun findOneById(id: ObjectId): Solve
    fun findAllByUserId(UserId: String): List<Solve>
    fun findAllBySessionId(sessionId: String): List<Solve>


    override fun deleteAll()
}