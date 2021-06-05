package pl.virstimer.api

import org.junit.jupiter.api.AfterEach
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.data.mongodb.core.query.Criteria
import org.springframework.data.mongodb.core.query.Query
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.db.security.model.User
import pl.virstimer.model.Session
import pl.virstimer.model.Solve
import pl.virstimer.model.Solved

@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
class SolveControllerTest : TestCommons(){
    @BeforeEach
    fun injections(){
        beforeEach()
        registerAndLogin()
    }

    @AfterEach
    fun after() {
        mongoTemplate.dropCollection(User::class.java)
    }

    @Test
    fun should_return_solve_for_user() {
        mockMvc.perform(MockMvcRequestBuilders.get("/solve/user/1").header("Authorization", token))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].sessionId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].scramble").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].time").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].timestamp").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].solved").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }

    @Test
    fun should_return_solve_for_session() {
        mockMvc.perform(MockMvcRequestBuilders.get("/solve/session/1").header("Authorization", token))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].sessionId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].scramble").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].time").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].timestamp").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].solved").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)
    }

    @Test
    fun should_patch_solve() {
        createSolve("user-id", "1", Solved.PLUS_TWO, token).andExpect(MockMvcResultMatchers.status().isCreated)
        val solve = mongoTemplate.find(Query(Criteria.where("userId").`is`("user-id")), Solve::class.java).first()
        assert(solve.solved == Solved.PLUS_TWO)

        patchSolve(solve.id, Solved.DNF, token).andExpect(MockMvcResultMatchers.status().isOk)
        assert(mongoTemplate.find(Query(Criteria.where("userId").`is`("user-id")), Solve::class.java).first().solved == Solved.DNF)
    }
}