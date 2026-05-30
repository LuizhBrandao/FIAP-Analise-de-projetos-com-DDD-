using Microsoft.AspNetCore.SignalR;

namespace TractorRental.Api.Hubs;

public class MonitoramentoHub : Hub
{
    // O SignalR gerencia as conexões automaticamente aqui.
    // Como nossa API apenas envia os alertas, não precisamos de métodos internos nesta classe.
}