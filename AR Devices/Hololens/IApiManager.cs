using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IApiManager  {

    void getHistory(HistoryListener listener);

    void turnOff(ActionListener listener);

    void turnOn(ActionListener listener);

    void getInstantValues(InstantValuesListener listener);
}
