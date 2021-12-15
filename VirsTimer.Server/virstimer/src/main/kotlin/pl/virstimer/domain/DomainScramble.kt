package pl.virstimer.domain

import org.worldcubeassociation.tnoodle.svglite.Svg

data class DomainScramble(
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
    THREE_BY_THREE_BLINDFOLDED,
    THREE_BY_THREE_OH,
    CLOCK,
    MEGAMINX,
    PYRAMINX,
    SKEWB,
    SQUARE_ONE,
    FOUR_BY_FOUR_BLINDFOLDED,
    FIVE_BY_FIVE_BLINDFOLDED,
}