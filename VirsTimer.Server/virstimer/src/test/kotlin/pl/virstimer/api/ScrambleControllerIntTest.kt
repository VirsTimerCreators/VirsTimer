package pl.virstimer.api

import org.junit.jupiter.api.BeforeEach
import org.junit.jupiter.api.Test
import org.junit.jupiter.api.extension.ExtendWith
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc
import org.springframework.boot.test.context.SpringBootTest
import org.springframework.test.annotation.DirtiesContext
import org.springframework.test.context.ActiveProfiles
import org.springframework.test.context.junit.jupiter.SpringExtension
import org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get
import org.springframework.test.web.servlet.result.MockMvcResultMatchers.jsonPath
import org.springframework.test.web.servlet.result.MockMvcResultMatchers.status
import pl.virstimer.TestCommons


@SpringBootTest
@ExtendWith(SpringExtension::class)
@AutoConfigureMockMvc
@ActiveProfiles("test")
//@DirtiesContext(classMode = DirtiesContext.ClassMode.AFTER_EACH_TEST_METHOD)
class ScrambleControllerIntTest : TestCommons(){

    @BeforeEach
    fun injections(){ before_each() }

    @Test
    fun should_return_scramble() {
        mockMvc.perform(get("/scramble/THREE_BY_THREE"))
                .andExpect(jsonPath("$[0].scramble").isNotEmpty)
                .andExpect(jsonPath("$[0].svgTag").isNotEmpty)
                .andExpect(status().isOk)
    }

    @Test
    fun should_return_400_if_puzzle_type_is_unused() {
        mockMvc.perform(get("/scramble/ABCD"))
                .andExpect(status().`is`(400))
    }
}