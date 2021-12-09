package pl.virstimer.repository.multiplayer

import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.model.multiplayer.PersistentScramble

@Repository
interface ScrambleRepository: MongoRepository<PersistentScramble, String>, ScrambleCustomRepository

interface ScrambleCustomRepository {
    fun findByIds(ids: Set<String>): List<PersistentScramble>
}

class ScrambleCustomRepositoryImpl(
    val mongoTemplate: MongoTemplate
) : ScrambleCustomRepository {
    override fun findByIds(ids: Set<String>): List<PersistentScramble> {
        return mongoTemplate.find(
            Query(Criteria.where("id").`in`(ids)),
            PersistentScramble::class.java
        )
    }
}

