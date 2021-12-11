package pl.virstimer.repository.multiplayer

import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.model.multiplayer.MultiplayerSolve

@Repository
interface MultiplayerSolveRepository : MongoRepository<MultiplayerSolve, String>, MultiplayerSolveCustomRepository {
    fun findAllByRoomId(roomId: String): List<MultiplayerSolve>
}

interface MultiplayerSolveCustomRepository {
    fun findByIds(userIds: Set<String>, roomId: String): List<MultiplayerSolve>
}

class MultiplayerSolveCustomRepositoryImpl(
    val mongoTemplate: MongoTemplate
) : MultiplayerSolveCustomRepository {
    override fun findByIds(userIds: Set<String>, roomId: String): List<MultiplayerSolve> {
        return mongoTemplate.find(
            Query(Criteria.where("userId").`in`(userIds).and("roomId").`is`(roomId)),
            MultiplayerSolve::class.java
        )
    }
}