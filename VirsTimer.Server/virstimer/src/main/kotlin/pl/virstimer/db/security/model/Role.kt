package pl.virstimer.db.security.model

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.Document

@Document(collection = "roles")
data class Role(
    @Id val id: String,
    val name: ERole
)

enum class ERole {
    ROLE_USER, ROLE_MODERATOR, ROLE_ADMIN
}