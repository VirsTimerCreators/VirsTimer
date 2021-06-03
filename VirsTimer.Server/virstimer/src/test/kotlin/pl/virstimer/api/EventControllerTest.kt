package pl.virstimer.api

import org.junit.jupiter.api.Test

import org.junit.jupiter.api.Assertions.*
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.data.annotation.Id
import org.springframework.data.mongodb.core.MongoTemplate
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders
import org.springframework.test.web.servlet.result.MockMvcResultMatchers
import pl.virstimer.domain.PuzzleType
import pl.virstimer.model.*
import pl.virstimer.TestHelper
@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
internal class EventControllerTest:TestHelper() {


    @BeforeEach
    fun before_all() {
        mongoTemplate.dropCollection(Event::class.java)
        mongoTemplate.dropCollection(Session::class.java)
        mongoTemplate.dropCollection(Solve::class.java)
        mongoTemplate.insertAll(
            listOf(
                Event( null,"1", "THREE_BY_THREE"),
                Event( null,"2", "FOUR_BY_FOUR"),
            )
        )
    }


    @Test
    fun posting_event_I_guess()
    {
        createEvent("1", "FOUR_BY_FOUR").andExpect(MockMvcResultMatchers.status().isOk)
    }
    @Test
    fun should_return_sessions() {
        mockMvc.perform(MockMvcRequestBuilders.get("/sessions/all"))
            .andExpect(MockMvcResultMatchers.jsonPath("$.id").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$.userId").isNotEmpty)
            .andExpect(MockMvcResultMatchers.jsonPath("$.puzzleType").isNotEmpty)
            .andExpect(MockMvcResultMatchers.status().isOk)

    }



    fun getRepository() {
    }
}