package pl.virstimer.api

import org.springframework.http.HttpStatus
import org.springframework.http.ResponseEntity
import org.springframework.web.bind.annotation.GetMapping
import org.springframework.web.bind.annotation.PathVariable
import org.springframework.web.bind.annotation.RequestParam
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
        @PathVariable puzzleType: PuzzleType, @RequestParam(required = false) amount: Int?
    ): ResponseEntity<MutableList<ScrambleResponse>> {
        val list = mutableListOf<ScrambleResponse>()
        if (amount != null) {
            repeat(amount) {
                list.add(scrambleService.generateScrambleAndSvg(puzzleType).toScrambleResponse())
            }

        }
        else list.add(scrambleService.generateScrambleAndSvg(puzzleType).toScrambleResponse())

        return ResponseEntity(list, HttpStatus.OK)
    }
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