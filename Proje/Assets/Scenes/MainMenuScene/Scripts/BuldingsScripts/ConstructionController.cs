using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ConstructionController : MonoBehaviour
{
    // Referansýnýzý ProgressBarController scriptine baðlayýn
    public ProgressBarController progressBarController;

    public bool CanStartNewConstruction()
    {
        // Tüm bool deðiþkenlerini bir listeye ekleyin
        bool[] activeConstructions = new bool[]
        {
            progressBarController.isBarracksBuildActive,
            progressBarController.isUnitCreationActive,
            progressBarController.isHealActive,
            progressBarController.isHospitalBuildActive,
            progressBarController.isTowerBuildingActive,
            progressBarController.isFarmBuildActive,
            progressBarController.isAnyTrapActive,
            progressBarController.isWareHouseBuildingActive,
            progressBarController.isStonePitBuildingActive,
            progressBarController.isSawmillBuildingActive,
            progressBarController.isBlacksmithBuildingActive,
            progressBarController.isCastleBuildingActive
        };

        // Aktif olan inþaatlarýn sayýsýný kontrol edin
        int activeCount = activeConstructions.Count(isActive => isActive);

        // Eðer aktif olan inþaat sayýsý 2 veya daha fazlaysa yeni inþaata izin verme
        return activeCount < 2;
    }
}
