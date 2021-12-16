package pl.virstimer.repository.multiplayer

import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.data.mongodb.core.query.Update
import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.api.multiplayer.RoomStatus
import pl.virstimer.model.multiplayer.PersistentRoom

@Repository
interface RoomRepository: MongoRepository<PersistentRoom, String>, RoomRepositoryCustom

interface RoomRepositoryCustom {
    fun modifyRoom(roomId: String, roomStatus: RoomStatus, userId: String): Boolean
    fun join(joinCode: String, userId: String): PersistentRoom?
    fun leave(roomId: String, userId: String): Boolean
    fun findByIdAndUser(roomId: String, userId: String): PersistentRoom?
}

class RoomRepositoryCustomImpl(
    val mongoTemplate: MongoTemplate
) : RoomRepositoryCustom {
    override fun modifyRoom(roomId: String, roomStatus: RoomStatus, userId: String): Boolean =
        mongoTemplate.updateFirst(
            Query(Criteria.where("administratorId").`is`(userId).and("id").`is`(roomId).and("status").ne(RoomStatus.CLOSED)),
            Update().set("status", roomStatus),
            PersistentRoom::class.java
        ).wasAcknowledged()

    override fun join(joinCode: String, userId: String): PersistentRoom? {
        return mongoTemplate.findAndModify(
            Query(Criteria.where("joinCode").`is`(joinCode).and("status").`is`(RoomStatus.OPEN).and("users").not().`in`(userId)),
            Update().addToSet("users", userId),
            PersistentRoom::class.java
        )
    }

    override fun leave(roomId: String, userId: String): Boolean {
        return mongoTemplate.updateFirst(
            Query(Criteria.where("id").`is`(roomId).and("users").`in`(userId)),
            Update().pull("users", userId),
            PersistentRoom::class.java
        ).wasAcknowledged()
    }

    override fun findByIdAndUser(roomId: String, userId: String): PersistentRoom? {
        return mongoTemplate.findOne(
            Query(Criteria.where("id").`is`(roomId).and("users").`in`(userId)),
            PersistentRoom::class.java
        )
    }
}