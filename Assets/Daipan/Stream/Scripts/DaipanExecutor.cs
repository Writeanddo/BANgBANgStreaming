﻿#nullable enable
namespace Daipan.Stream.Scripts
{
    public class DaipanExecutor
    {
        readonly DaipanParameter _daipanParameter;
        readonly ViewerNumber _viewerNumber;
        readonly StreamStatus _streamStatus;

        public DaipanExecutor(
            DaipanParameter daipanParameter,
            ViewerNumber viewerNumber,
            StreamStatus streamStatus)
        {
            _daipanParameter = daipanParameter;
            _viewerNumber = viewerNumber;
            _streamStatus = streamStatus;
        }

        bool IsExciting => _streamStatus.IsExcited;

        public void DaiPan()
        {
            if (IsExciting)
                _viewerNumber.IncreaseViewer(_daipanParameter.increaseNumberWhenExciting);
            else
                _viewerNumber.IncreaseViewer(_daipanParameter.increaseNumberByDaipan);
        }
    }
}