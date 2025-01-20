
    using Microsoft.AspNetCore.Mvc;

    namespace PositionsAPI.Controllers{
        
        [ApiController]
        [Route("api/[controller]")]
        public class PositionsController : ControllerBase{
            
            private readonly IPositionService _positionService;
            private readonly ILogger<PositionsController> _logger;

            public PositionsController(IPositionService positionService, ILogger<PositionsController> logger){
                
                _positionService = positionService;
                _logger = logger; 
            }

            [HttpGet]
            public IActionResult GetPositions(){
                
                try{ 
                    _logger.LogInformation("Fetching all positions.");
                    var positions = _positionService.GetAllPositions();
                    return Ok(positions);
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "An error occurred while fetching positions.");
                    return StatusCode(500, new { message = "Error fetching positions.", error = ex.Message });
                }
            }

            [HttpPost]
            public IActionResult CreatePosition([FromBody] Position position){
                
                try{
                    _logger.LogInformation("Creating new position: {PositionName}", position.Name);
                    _positionService.AddPosition(position);
                    
                    return CreatedAtAction(nameof(GetPositions), new { name = position.Name }, position);
                }
                catch (ArgumentException ex){
                    
                    _logger.LogWarning(ex, "Invalid position data: {PositionName}", position.Name);
                    return BadRequest(new { message = ex.Message });
                }
            }

            [HttpGet("{name}/distance")]
            public IActionResult GetDistances(string name){
                
                try{
                    _logger.LogInformation("Calculating distances for position: {PositionName}", name);
                    var distances = _positionService.CalculateDistances(name);
                    return Ok(distances);
                }
                catch (KeyNotFoundException ex) {
                    _logger.LogWarning(ex, "Position not found: {PositionName}", name);
                    return NotFound(new { message = ex.Message });
                }
            }
        }
    }
