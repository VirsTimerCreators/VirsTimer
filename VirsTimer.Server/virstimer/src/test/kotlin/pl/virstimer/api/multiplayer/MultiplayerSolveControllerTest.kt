package pl.virstimer.api.multiplayer

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.ActiveProfiles
import org.springframework.test.context.junit.jupiter.SpringExtension
import pl.virstimer.TestCommons
import pl.virstimer.model.Solved
import pl.virstimer.model.multiplayer.MultiplayerSolve

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
@ActiveProfiles("test")
class MultiplayerSolveControllerTest : TestCommons() {

    @BeforeEach
    fun beforeEach() {
        before_each()
    }

    @Test
    fun `should create multiplayerSolve`() {
        val loginDetails = registerAndLogin("user-1", "user-1-pass")

        createMultiplayerSolve(
            "roomId-solve",
            "scrambleId-solve",
            71531638715908381,
            440,
            Solved.OK,
            loginDetails.authHeader
        )

        val solves = mongoTemplate.findAll(MultiplayerSolve::class.java)
        assert(solves.last().scrambleId == "scrambleId-solve")

    }

}