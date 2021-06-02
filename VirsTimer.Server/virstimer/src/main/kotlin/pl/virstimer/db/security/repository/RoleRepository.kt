package pl.virstimer.db.security.repository

import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.db.security.model.ERole
import pl.virstimer.db.security.model.Role


@Repository
interface RoleRepository : MongoRepository<Role, String> {
    fun findByName(name: ERole?): Role?
}