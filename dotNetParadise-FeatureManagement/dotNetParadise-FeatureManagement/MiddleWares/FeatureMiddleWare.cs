namespace dotNetParadise_FeatureManagement.MiddleWares;

public class FeatureMiddleWare(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        Console.WriteLine("FeatureMiddleWare管道执行之前~");
        await next(context);
        Console.WriteLine("FeatureMiddleWare管道执行之后~");
    }
}
