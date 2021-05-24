package pl.virstimer.db.security.repository

import org.springframework.data.mongodb.repository.MongoRepository
import org.springframework.stereotype.Repository
import pl.virstimer.db.security.model.Role
import pl.virstimer.db.security.model.RoleName

@Repository
interface RoleRepository : MongoRepository<Role, String> {
    fun findByRoleName(roleName: RoleName): Role?
}