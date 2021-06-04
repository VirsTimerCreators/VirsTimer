package pl.virstimer.security

import org.springframework.security.core.userdetails.UserDetails
import org.springframework.security.core.userdetails.UserDetailsService
import org.springframework.security.core.userdetails.UsernameNotFoundException
import org.springframework.stereotype.Component
import org.springframework.transaction.annotation.Transactional
import pl.virstimer.db.security.model.User
import pl.virstimer.db.security.repository.UserRepository


@Component
class UserDetailsServiceImpl(
    val userRepository: UserRepository
) : UserDetailsService  {

    @Transactional
    override fun loadUserByUsername(username: String): UserDetails {
        val user: User = userRepository.findByUsername(username) ?: throw UsernameNotFoundException("User Not Found with username: $username")

        return UserDetailsImpl.build(user)
    }
}