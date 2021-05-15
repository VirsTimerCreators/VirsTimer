package pl.virstimer.domain

import org.worldcubeassociation.tnoodle.svglite.Svg

data class DomainScramble (
    val scrambleString: String,
    val scrambleSvg: Svg,
    val puzzleType: PuzzleType
)

enum class PuzzleType {
    THREE_BY_THREE
}