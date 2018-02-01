using eios.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace eios.Data
{
    public class TaskWeb
    {
        public async Task RunGetMarks(CancellationToken token)
        {
            await Task.Run(async () =>
            {
                while (true)
                {
                    token.ThrowIfCancellationRequested();

                    var message = new MarksMessage()
                    {
                        Message = await WebApi.Instance.GetMarksAsync(1)
                    };

                    Device.BeginInvokeOnMainThread(() => {
                        MessagingCenter.Send<MarksMessage>(message, "Marks");
                    });

                    await Task.Delay(5000);
                }
            }, token);
        }
    }
}
