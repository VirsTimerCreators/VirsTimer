package pl.virstimer.api

import org.bson.types.ObjectId
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.TestCommons
import pl.virstimer.model.Session

internal class SolveControllerTest : TestCommons(){

    @BeforeEach
    fun injections(){
        before_each() }
    // TODO  lateinit property mongoTemplate has not been initialized??? ONLY FOR SOLVES

    @Test
    fun should_return_solves() {
        mongoTemplate.insert(Session(ObjectId(),"1", "1","session_name1"))
        mockMvc.perform(MockMvcRequestBuilders.get("/solves/all"))
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
    fun should_return_solve_for_user() {
        mockMvc.perform(MockMvcRequestBuilders.get("/solves/user/1"))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].sessionId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].scramble").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].time").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].timestamp").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].solved").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }    @Test
    fun should_return_solve_for_session() {
        mockMvc.perform(MockMvcRequestBuilders.get("/solves/session/1"))
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].sessionId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].scramble").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].time").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].timestamp").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$[0].solved").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)
    }
}