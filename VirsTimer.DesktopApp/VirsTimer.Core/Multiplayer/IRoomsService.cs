﻿using System;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Scrambles;

namespace VirsTimer.Core.Multiplayer
{
    public interface IRoomsService
    {
        public Task<RepositoryResponse<Room>> CreateRoomAsync(string eventName, int solvesAmount);
        public Task<RepositoryResponse<Room>> JoinRoomAsync(string accessCode);
        public Task<RepositoryResponse> SendScrambleAsync(Scramble scramble);
        public IObservable<RoomNotification> Notifications { get; }
    }
}