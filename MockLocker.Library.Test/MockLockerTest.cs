using MockLocker.Library.Models;

namespace MockLocker.Library.Test
{
    public class MockLockerTest
    {
        [Theory]

        [InlineData(LockType.Lock16, 1, true)]
        [InlineData(LockType.Lock32, 32, true)]
        [InlineData(LockType.Lock48, 48, true)]
        [InlineData(LockType.Lock64, 64, true)]

        [InlineData(LockType.Lock16, 17, false)]
        [InlineData(LockType.Lock32, 33, false)]
        [InlineData(LockType.Lock48, 49, false)]
        [InlineData(LockType.Lock64, 65, false)]

        [InlineData(LockType.Lock16, 0, false)]
        [InlineData(LockType.Lock16, -1, false)]
        public void UnLockTest(LockType lockType, int locNo, bool expectedIsSuccess)
        {
            //Arrange
            var mockLocker = new MockLocker(lockType);

            //Act
            var response = mockLocker.Unlock(locNo);

            //Assert
            Assert.Equal(expectedIsSuccess, response.IsSuccess);

            //Act
            if (response.IsSuccess)
            {
                CheckStatusResponse statusResponse = mockLocker.CheckStatus();
                var lockNoStatus = statusResponse.Statuses.Where(x => x.LockNo == locNo).SingleOrDefault();

                //Assert
                Assert.False(lockNoStatus.IsLocked);
            }
        }


        [Theory]
        [InlineData(LockType.Lock16, true)]
        public void CheckStatusTest(LockType lockType, bool expectedIsSuccess)
        {
            //Arrange 
            var mockLocker = new MockLocker(lockType);

            //Act
            var response = mockLocker.CheckStatus();

            //Assert
            Assert.Equal(expectedIsSuccess, response.IsSuccess);
        }

        [Theory]
        [InlineData(LockType.Lock16)]
        [InlineData(LockType.Lock32)]
        [InlineData(LockType.Lock48)]
        [InlineData(LockType.Lock64)]
        public void CloseAllLocksTest(LockType lockType)
        {
            //Arrange
            var mockLocker = new MockLocker(lockType);
            //Act
            var response = mockLocker.CloseAllLocks();

            //Assert
            Assert.Equal(true, response.IsSuccess);

            var statusResponse = mockLocker.CheckStatus();

            var isAllLocksClosed = statusResponse.Statuses.All(x => x.IsLocked);
            Assert.Equal(true, isAllLocksClosed);
        }
    }
}