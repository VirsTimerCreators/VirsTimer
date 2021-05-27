package pl.virstimer.domain

import org.worldcubeassociation.tnoodle.svglite.Svg

data class DomainScramble (
    val scrambleString: String,
    val scrambleSvg: Svg,
    val puzzleType: PuzzleType
)

enum class PuzzleType {
    TWO_BY_TWO,
    THREE_BY_THREE,
    FOUR_BY_FOUR,
    FIVE_BY_FIVE,
    SIX_BY_SIX,
    SEVEN_BY_SEVEN,
    CLOCK,
    MEGAMINX,
    PYRAMINX,
    SKWEB,
    SQUARE_ONE,
}