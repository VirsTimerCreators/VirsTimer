package pl.virstimer.db.security.repository

import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.db.security.model.User

@Repository
interface UserRepository: MongoRepository<User, String> {

    fun findByUsername(username: String): User?
    fun existsByUsername(username: String): Boolean
    fun existsByEmail(email: String): Boolean
}