package pl.virstimer.security

import javax.validation.constraints.Email
import javax.validation.constraints.NotBlank
import javax.validation.constraints.Size


class LoginRequest {
    var username: @NotBlank String? = null
    var password: @NotBlank String? = null
}

data class SignupRequest (
    @NotBlank
    @Size(min = 3, max = 20)
    val username: String,

    @NotBlank
    @Size(max = 50)
    @Email
    val email: String,

    var roles: Set<String>,

    @NotBlank
    @Size(min = 6, max = 40)
    val password: String
) {
    fun setRole(roles: Set<String>) {
        this.roles = roles
    }
}


class JwtResponse(
    var accessToken: String,
    var id: String,
    var username: String,
    var email: String,
    val roles: List<String>
) {
    var tokenType = "Bearer"
}

class MessageResponse(var message: String)