using System.Linq;
using Interfaces;
using UnityEngine;

namespace Services
{
    public class PlatformManager : IPlatformManager
    {
        public int PlatformsCount { get; private set; }

        public PlatformManager()
        {
            PlatformsCount = 0;
        }

        public void StopPlatform(int? platformNumber = null)
        {
            var allPlatforms = Object.FindObjectsOfType<MovingPlatform>();

            var platform = platformNumber == null
                ? allPlatforms.First()
                : allPlatforms.ElementAtOrDefault(platformNumber.Value);

            if (platform == null)
                return;

            platform.initialSpeed = 0;
        }

        //TODO: s: refactor it please
        //TODO: s: write test for it
        public void CutPlatform()
        {
            var currentPlatform = Object.FindObjectsOfType<MovingPlatform>().First();
            var lastPlatform = Object.FindObjectsOfType<MovingPlatform>().ElementAtOrDefault(1);
            var main = GameObject.Find("MainCube");
            if (PlatformsCount == 1)
            {
                var hangoverMain = currentPlatform.transform.position.z - main.transform.position.z;
                var newMainSize = main.transform.localScale.z - Mathf.Abs(hangoverMain);
                var newMainPos = main.transform.position.z + (hangoverMain / 2);
                currentPlatform.transform.localScale = new Vector3(currentPlatform.transform.localScale.x,
                    currentPlatform.transform.localScale.y, newMainSize);
                currentPlatform.transform.position = new Vector3(currentPlatform.transform.position.x,
                    currentPlatform.transform.position.y, newMainPos);
            }
            else if (IsAxisZPlatform(PlatformsCount))
            {
                var hangoverZ = currentPlatform.transform.position.z - lastPlatform.transform.position.z;
                var newZSize = lastPlatform.transform.localScale.z - Mathf.Abs(hangoverZ);
                var newZPos = lastPlatform.transform.position.z + (hangoverZ / 2);
                currentPlatform.transform.localScale = new Vector3(currentPlatform.transform.localScale.x,
                    currentPlatform.transform.localScale.y, newZSize);
                currentPlatform.transform.position = new Vector3(currentPlatform.transform.position.x,
                    currentPlatform.transform.position.y, newZPos);

                var hangoverX = currentPlatform.transform.position.x - lastPlatform.transform.position.x;
                var newXSize = lastPlatform.transform.localScale.x - Mathf.Abs(hangoverX);
                var newXPos = lastPlatform.transform.position.x + (hangoverX / 2);
                currentPlatform.transform.localScale = new Vector3(newXSize, currentPlatform.transform.localScale.y,
                    currentPlatform.transform.localScale.z);
                currentPlatform.transform.position = new Vector3(newXPos, currentPlatform.transform.position.y,
                    currentPlatform.transform.position.z);
            }
            else if (!IsAxisZPlatform(PlatformsCount))
            {
                var hangoverZ1 = currentPlatform.transform.position.z - lastPlatform.transform.position.z;
                var newZSize1 = lastPlatform.transform.localScale.z - Mathf.Abs(hangoverZ1);
                var newZPos1 = lastPlatform.transform.position.z + (hangoverZ1 / 2);
                currentPlatform.transform.localScale = new Vector3(currentPlatform.transform.localScale.x,
                    currentPlatform.transform.localScale.y, newZSize1);
                currentPlatform.transform.position = new Vector3(currentPlatform.transform.position.x,
                    currentPlatform.transform.position.y, newZPos1);

                var hangoverX1 = currentPlatform.transform.position.x - lastPlatform.transform.position.x;
                var newXSize1 = lastPlatform.transform.localScale.x - Mathf.Abs(hangoverX1);
                var newXPos1 = lastPlatform.transform.position.x + (hangoverX1 / 2);
                currentPlatform.transform.localScale = new Vector3(newXSize1, currentPlatform.transform.localScale.y,
                    currentPlatform.transform.localScale.z);
                currentPlatform.transform.position = new Vector3(newXPos1, currentPlatform.transform.position.y,
                    currentPlatform.transform.position.z);
            }
        }

        public void CreatePlatform(GameObject platform)
        {
            var previousPlatform = Object.FindObjectsOfType<MovingPlatform>()?.FirstOrDefault();
            var position = GetPlatformInitialPosition(PlatformsCount);
            var instance = Object.Instantiate(platform, position, Quaternion.identity).GetComponent<MovingPlatform>();
            instance.isSpeedAxisZ = IsAxisZPlatform(PlatformsCount);
            instance.initialSpeed = Constants.MovingPlatform.InitialSpeed;
            instance.transform.localScale = previousPlatform != null
                ? previousPlatform.transform.localScale
                : instance.transform.localScale;
            PlatformsCount++;
        }
        
        public bool PlatformMissed()
        {
            var platforms = Object.FindObjectsOfType<MovingPlatform>();
            var current = platforms.First();
            var prev = platforms.ElementAtOrDefault(1);
            var cur = current.transform.position.x;
            return false;
        }

        #region Private Methods

        private Vector3 GetPlatformInitialPosition(int platformNumber)
        {
            var previousPlatform = Object.FindObjectsOfType<MovingPlatform>()?.FirstOrDefault();
            if (previousPlatform == null)
                return IsAxisZPlatform(platformNumber)
                    ? new Vector3(Constants.MovingPlatform.InitialPosX,
                        Constants.MovingPlatform.InitialPosY + platformNumber * Constants.MovingPlatform.InitialScaleY,
                        Constants.MovingPlatform.InitialPosZ)
                    : new Vector3(Constants.MovingPlatform.InitialPosZ,
                        Constants.MovingPlatform.InitialPosY + platformNumber * Constants.MovingPlatform.InitialScaleY,
                        Constants.MovingPlatform.InitialPosX);

            return IsAxisZPlatform(platformNumber)
                ? new Vector3(previousPlatform.transform.position.x,
                    Constants.MovingPlatform.InitialPosY + platformNumber * Constants.MovingPlatform.InitialScaleY,
                    Constants.MovingPlatform.InitialPosZ)
                : new Vector3(Constants.MovingPlatform.InitialPosZ,
                    Constants.MovingPlatform.InitialPosY + platformNumber * Constants.MovingPlatform.InitialScaleY,
                    previousPlatform.transform.position.z);
        }

        private bool IsAxisZPlatform(int platformNumber)
        {
            return platformNumber % 2 == 0;
        }
        
        #endregion
    }
}