using ConfigData;
using Core;
using GameUnits;
using UI;
using UnityEngine;
using Zenject;

namespace GameServices
{
    public class LevelLoader : MonoBehaviour
    {
        [SerializeField] private Vector3 _carStartPos = new Vector3(0, 0.5f, -48.6f);

        private PlayerCar _car;
        private GroundsController _groundsController;
        private ResultPopup _resultPopup;
        private EnemyCreator _enemyCreator;
        private UnitsConfig _unitsConfig;

        private ProgressBar _progressBar;

        public bool IsPaused { get; private set; } = true;

        [Inject]
        public void Construct(PlayerCar playerCar, GroundsController groundsController, ResultPopup resultPopup,
            EnemyCreator enemyCreator, ConfigProvider configProvider, ProgressBar progressBar)
        {
            _progressBar = progressBar;
            _car = playerCar;
            _groundsController = groundsController;
            _resultPopup = resultPopup;
            _enemyCreator = enemyCreator;
            _unitsConfig = configProvider.UnitConfig;
        }

        private void Start()
        {
            _progressBar.OnFinish += OnWin;
            _resultPopup.OnStartClicked += Restart;
            _car.OnDied += ShowLosePopup;
        }

        private void OnDestroy()
        {
            _progressBar.OnFinish -= OnWin;
            _resultPopup.OnStartClicked -= Restart;
            _car.OnDied -= ShowLosePopup;
        }

        public void Restart()
        {
            IsPaused = false;
            _groundsController.Restart();
            ResetCar();
            _progressBar.Setup(_unitsConfig.GetDefaultLevelModel, _carStartPos.z, _car.transform);
            _enemyCreator.RefreshLevel();
            _car.StartLevel();
        }

        private void ResetCar()
        {
            _car.transform.position = _carStartPos;
            var carModel = _unitsConfig.GetUnitModel(UnitType.Player);
            var turretModel = _unitsConfig.GetTurretModel(0);
            _car.InitUnit(carModel, turretModel);
        }

        private void ShowLosePopup()
        {
            _enemyCreator.Clear();
            Pause();
            _resultPopup.ShowResult(false);
        }

        private void OnWin()
        {
            _enemyCreator.Clear();
            Pause();
            _resultPopup.ShowResult(true);
        }

        public void Pause()
        {
            IsPaused = true;
            _car.Stop();
            _enemyCreator.Stop();
            _groundsController.OnPause();
        }

        public void Resume()
        {
            IsPaused = false;
            _car.StartLevel();
            _enemyCreator.Resume();
            _groundsController.OnResume();
        }
    }
}