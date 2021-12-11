using Microsoft.AspNetCore.Mvc;
using server.Inputs;
using server.Interfaces;
using server.Payloads;

namespace server.Controllers;

public class JobController : BaseController
{
    private readonly IJobService _jobService;

    public JobController(IJobService jobService)
    {
        _jobService = jobService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobPayload>>> GetJobs([FromQuery] int skip = 0, [FromQuery] int take = 10)
    {
        return Ok(await _jobService.jobRepository.GetJobPayloads(skip, take));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JobPayload>> GetJob([FromRoute] Guid id, [FromQuery] int take = 10)
    {
        var job = await _jobService.jobRepository.GetJobPayloadByIdWithLogs(id);
        return job is null ? NotFound() : Ok(job);
    }

    [HttpPost]
    public async Task<ActionResult<JobPayload>> AddJob([FromBody] JobInput input)
    {
        var job = await _jobService.AddJob(input);
        return await _jobService.Complete() ? Ok(new JobPayload(job)) : BadRequest();
    }

    [HttpPut]
    public async Task<ActionResult> ProcessJobs()
    {
        var jobs = await _jobService.ProcessJobs();
        if (!(await _jobService.Complete())) return BadRequest(new { message = "error while processing jobs" });
        await _jobService.CompleteJobs(jobs);
        if (!(await _jobService.Complete())) return BadRequest(new { message = "error while completing jobs" });
        return Ok(new { message = "All jobs were processed." });
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<JobPayload>> RemoveJob([FromRoute] Guid id)
    {
        var job = await _jobService.jobRepository.RemoveJob(id);
        if (job is null) return NotFound();
        return await _jobService.Complete() ? Ok(new JobPayload(job)) : BadRequest();
    }
}
