package pl.virstimer.repository

import org.bson.types.ObjectId
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.core.query.Update
import org.springframework.data.mongodb.core.query.UpdateDefinition
import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.model.Session
import pl.virstimer.model.Solve
import pl.virstimer.model.SolveChange

interface SolveRepository : MongoRepository<Solve, ObjectId> {
    fun findOneByIdAndUserId(id: ObjectId, userId: String): Solve
    fun findAllByUserId(userId: String): List<Solve>
    fun findAllBySessionIdAndUserId(sessionId: String, userId: String): List<Solve>
    fun deleteSolveByIdAndUserId(id:ObjectId, userId: String)
    fun deleteSolveByUserId(userId: String)
}

interface SolveCustomRepository{
    fun updateSolve(id: ObjectId, solveUpdate: SolveChange, userId: String)
}

@Repository
class SolveCustomRepositoryImpl(
    val mongoTemplate: MongoTemplate
):SolveCustomRepository{
    override fun updateSolve(id: ObjectId, solveUpdate: SolveChange, userId: String) {
       mongoTemplate.updateFirst(
           Query(
               Criteria("_id").`is`(id).and("userId").`is`(userId)),
           Update().set("solved", solveUpdate.solved),
           Solve::class.java
       )
    }
}