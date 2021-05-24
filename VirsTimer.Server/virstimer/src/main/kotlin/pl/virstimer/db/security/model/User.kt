package pl.virstimer.db.security.model

import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.mapping.DBRef
import org.springframework.data.mongodb.core.mapping.Document
import java.util.*
import javax.validation.constraints.Email
import javax.validation.constraints.NotBlank
import javax.validation.constraints.Size


@Document(collection = "users")
class User {
    @Id
    var id: String? = null
    var username: @NotBlank @Size(max = 20) String? = null

    @Email
    var email: @NotBlank @Size(max = 50) String? = null
    var password: @NotBlank @Size(max = 120) String? = null

    @DBRef
    var roles: Set<Role> = HashSet()

    constructor() {}
    constructor(username: String?, email: String?, password: String?) {
        this.username = username
        this.email = email
        this.password = password
    }
}