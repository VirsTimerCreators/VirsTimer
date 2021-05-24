package pl.virstimer.db.security.model

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document

@Document(collection = "roles")
data class Role(
    @Id val id: String,
    val roleName: RoleName
)

enum class RoleName {
    USER,
    MODERATOR,
    ADMIN
}