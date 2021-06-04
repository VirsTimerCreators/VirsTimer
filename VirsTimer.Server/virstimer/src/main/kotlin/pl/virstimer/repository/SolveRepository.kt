package pl.virstimer.repository

import org.bson.types.ObjectId
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.core.query.Update
import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.model.Solve
import pl.virstimer.model.SolveChange

interface SolveRepository : MongoRepository<Solve, ObjectId> {
    fun findOneById(id: String): Solve
    fun findAllByUserId(UserId: String): List<Solve>
    fun findAllBySessionId(sessionId: String): List<Solve>
    fun deleteSolveById(id: String)

    override fun deleteAll()
}

interface SolveCustomRepository {
    fun updateSolve(id: String, solveUpdate: SolveChange)
}

@Repository
class SolveCustomRepositoryImpl(
    val mongoTemplate: MongoTemplate
) : SolveCustomRepository {
    override fun updateSolve(id: String, solveUpdate: SolveChange) {
        mongoTemplate.updateFirst(
            Query(Criteria("_id").`is`(id)),
            Update().set("solved", solveUpdate.solved),
            Solve::class.java
        )
    }
}