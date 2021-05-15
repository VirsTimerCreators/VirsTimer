package pl.virstimer.api

import org.springframework.stereotype.Controller
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RestController
import pl.virstimer.domain.DomainScramble
import pl.virstimer.domain.PuzzleType
import pl.virstimer.service.ScrambleService

@RestController
class ScrambleController(
        val scrambleService: ScrambleService
) {

    @GetMapping("/scramble/{puzzleType}")
    fun getScramble(
            @PathVariable puzzleType: PuzzleType
    ): ScrambleResponse =
            scrambleService.generateScrambleAndSvg(puzzleType).toScrambleResponse()


}

data class ScrambleResponse(
        val scramble: String,
        val svgTag: String
)

private fun DomainScramble.toScrambleResponse() =
        ScrambleResponse(
                this.scrambleString,
                this.scrambleSvg.toString()
        )