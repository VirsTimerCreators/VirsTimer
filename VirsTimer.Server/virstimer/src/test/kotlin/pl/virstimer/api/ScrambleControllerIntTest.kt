package pl.virstimer.api

import com.fasterxml.jackson.databind.ObjectMapper
import org.hamcrest.core.Is.`is`
import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.beans.factory.annotation.Autowired
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.MockMvc
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get
import org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath
import org.springframework.test.web.servlet.result.MockMvcResultMatchers.status
import pl.virstimer.TestHelper
import pl.virstimer.model.Event
import pl.virstimer.model.Session
import pl.virstimer.model.Solve
import pl.virstimer.model.Solved


@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
class ScrambleControllerIntTest: TestHelper(){

    @BeforeEach
    fun injections(){ before_all() }


    @Test
    fun should_return_scramble() {
        mockMvc.perform(get("/scramble/THREE_BY_THREE"))
                .andExpect(jsonPath("$.scramble").isNotEmpty)
                .andExpect(jsonPath("$.svgTag").isNotEmpty)
                .andExpect(status().isOk)

    }

    @Test
    fun should_return_400_if_puzzle_type_is_unused() {
        mockMvc.perform(get("/scramble/ABCD"))
                .andExpect(status().`is`(400))
    }
}