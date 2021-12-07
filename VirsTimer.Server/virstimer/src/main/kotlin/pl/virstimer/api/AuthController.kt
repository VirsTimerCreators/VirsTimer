package pl.virstimer.api

import org.slf4j.LoggerFactory
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.http.ResponseEntity
import org.springframework.security.authentication.AuthenticationManager
import org.springframework.security.authentication.UsernamePasswordAuthenticationToken
import org.springframework.security.core.Authentication
import org.springframework.security.core.GrantedAuthority
import org.springframework.security.core.context.SecurityContextHolder
import org.springframework.security.crypto.password.PasswordEncoder
import org.springframework.web.bind.annotation.*
import pl.virstimer.db.security.model.ERole
import pl.virstimer.db.security.model.Role
import pl.virstimer.db.security.model.User
import pl.virstimer.db.security.repository.RoleRepository
import pl.virstimer.db.security.repository.UserRepository
import pl.virstimer.security.*
import pl.virstimer.service.EventService
import java.util.*
import java.util.function.Consumer
import java.util.stream.Collectors
import javax.validation.Valid


@CrossOrigin(origins = ["*"], maxAge = 3600)
@RestController
@RequestMapping("/api/auth")
class AuthController(
    @Autowired
    var authenticationManager: AuthenticationManager,
    @Autowired
    var userRepository: UserRepository,
    @Autowired
    var roleRepository: RoleRepository,
    @Autowired
    val eventService: EventService,
    @Autowired
    var encoder: PasswordEncoder,
    @Autowired
    var jwtUtils: JwtUtils,

    var logger: org.slf4j.Logger = LoggerFactory.getLogger(AuthController::class.java)

) {


    @PostMapping("/signin")
    fun authenticateUser(@Valid @RequestBody loginRequest: LoginRequest): ResponseEntity<*> {
        val authentication: Authentication = authenticationManager.authenticate(
            UsernamePasswordAuthenticationToken(loginRequest.username, loginRequest.password)
        )
        SecurityContextHolder.getContext().authentication = authentication
        val jwt = jwtUtils.generateJwtToken(authentication)
        val userDetails = authentication.principal as UserDetailsImpl
        val roles = userDetails.authorities.stream()
            .map { item: GrantedAuthority -> item.authority }
            .collect(Collectors.toList())
        return ResponseEntity.ok<Any>(
            JwtResponse(
                jwt,
                userDetails.getId(),
                userDetails.username,
                userDetails.getEmail(),
                roles
            )
        )
    }

    @PostMapping("/signup")
    fun registerUser(@Valid @RequestBody signUpRequest: SignupRequest): ResponseEntity<*> {
        if (userRepository.existsByUsername(signUpRequest.username)) {
            return ResponseEntity
                .badRequest()
                .body<Any>(MessageResponse("Error: Username is already taken!"))
        }
        if (userRepository.existsByEmail(signUpRequest.email)) {
            return ResponseEntity
                .badRequest()
                .body<Any>(MessageResponse("Error: Email is already in use!"))
        }

        // Create new user's account
        val user = User(
            signUpRequest.username,
            signUpRequest.email,
            encoder.encode(signUpRequest.password)
        )

        val strRoles: Set<String> = signUpRequest.roles
        val roles: MutableSet<Role> = HashSet()

        if (strRoles == null) {
            val userRole: Role = roleRepository.findByName(ERole.ROLE_USER) ?: throw RuntimeException("Error: Role is not found.")
            roles.add(userRole)
        } else {
            strRoles.forEach(Consumer { role: String? ->
                when (role) {
                    "admin" -> {
                        val adminRole: Role = roleRepository.findByName(ERole.ROLE_ADMIN) ?: throw RuntimeException("Error: Role is not found.")
                        roles.add(adminRole)
                    }
                    "mod" -> {
                        val modRole: Role = roleRepository.findByName(ERole.ROLE_MODERATOR) ?: throw RuntimeException("Error: Role is not found.")
                        roles.add(modRole)
                    }
                    else -> {
                        val userRole: Role = roleRepository.findByName(ERole.ROLE_USER) ?: throw RuntimeException("Error: Role is not found.")
                        roles.add(userRole)
                    }
                }
            })
        }

        user.roles = roles
        userRepository.save(user)

        if(user.username == null) { throw RuntimeException("error: username is null")}
        eventService.createEvents(user.username!!)

        return ResponseEntity.ok(MessageResponse("User registered successfully!"))
    }
}